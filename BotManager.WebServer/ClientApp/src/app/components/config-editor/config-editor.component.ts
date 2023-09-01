import {Component, Input} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {ConfigTypes} from "../../controllers/config-types";
import {NestedTreeControl} from "@angular/cdk/tree";
import {MatTreeNestedDataSource} from "@angular/material/tree";
import {TypePropertyInfoDto} from "../../dto/type-property-info-dto";

interface ConfigNode {
  name: string;
  value?: any;
  type?: TypeInfoDto;
  targetType?: TypeInfoDto;
  documentationXml?: string;
  isProperty: boolean;
  isList: boolean;
  children?: ConfigNode[];
  parent?: ConfigNode;
}

@Component({
  selector: 'app-config-editor',
  templateUrl: './config-editor.component.html',
  styleUrls: ['./config-editor.component.css']
})
export class ConfigEditorComponent {
  constructor() { }

  /**
  * The type definitions.
  */
  private _types: ConfigTypes | undefined;

  @Input()
  public set types(value: ConfigTypes | undefined) {
    this._types = value;
    this.updateTargetType();
  }
  public get types(): ConfigTypes | undefined {
    return this._types;
  }

  /**
   * The value to edit.
   */
  private _value: any;
  @Input()
  public set value(value: any) {
    this._value = value;
    this.updateTargetType();
  }

  public get value(): any {
    return this._value;
  }

  /**
   * The target type of the root element. This should always be a $List.
   */
  public targetType: TypeInfoDto | undefined;

  /**
   * Sets the root target type. This should load the $List type.
   */
  private updateTargetType() {
    // Ensure the root element is a list. A list element as root is useful, so you can add new expressions easier.
    if (this._value) {
      if (!Array.isArray(this._value)) {
        this._value = [this._value];
      }
    }

    const targetType = this.types?.getTypeByExpressionName("$List");
    const node = this.buildNode(this._value, targetType?.typeName, undefined);
    this.dataSource.data = [node];
  }


  public root?: ConfigNode;
  treeControl = new NestedTreeControl<ConfigNode>(node => node.children);
  dataSource = new MatTreeNestedDataSource<ConfigNode>();

  private buildNode(value: any, targetTypeName?: string, parent?: ConfigNode): ConfigNode {
    const targetType = targetTypeName ? this.types?.getTypeByTypeName(targetTypeName) : undefined;
    const type = this.types?.getTypeByObject(value, targetType) ?? targetType;

    const node: ConfigNode = {
      name: this.getDisplayNameFromType(type),
      value: value,
      parent: parent,
      type: type,
      targetType: targetType,
      documentationXml: type?.documentationXml,
      isProperty: false,
      isList: false,
    };

    if (type) {
      if (type.isList) {
        node.children = [];
        node.isList = true;
        if (value) {
          const elementType = this.types?.getListElementType(type);
          for (const child of value) {
            node.children.push(this.buildNode(child, elementType?.typeName, node));
          }
        }
      } else if (type.isObject) {
        node.children = [];
        if (value) {
          for (const property of type.properties) {
            const propertyType = this.types?.getTypeByTypeName(property.typeName);
            const propertyValue = this.getPropertyValue(value, property);
            const propertyNode = this.buildNode(propertyValue, propertyType?.typeName, node);
            propertyNode.name = property.name;
            propertyNode.documentationXml = property.documentationXml;
            node.children.push(propertyNode);
          }
        }
      }
    }

    return node;
  }

  /**
   * Returns if this node is a list.
   */
  public nodeIsList = (node: ConfigNode) => node.isList;

  /**
   * Returns if this node is a list element.
   */
  public nodeIsListElement = (node: ConfigNode) => node.parent?.isList;

  /**
   * Returns if this node can change it's type and the settings button should be visible.
   */
  public nodeCanChangeType = (node: ConfigNode) => node.targetType?.isObject;

  /**
   * Returns if this node has a visible expand button.
   */
  public nodeHasExpandButton = (node: ConfigNode) => node.type?.isObject && node.value !== null;

  /**
   * Returns if this node is visualized by an input field.
   */
  public nodeIsInput = (node: ConfigNode) => node.type?.isNative && node.type?.typeName !== 'System.Boolean' &&
    node.value !== null;

  /**
   * Returns if this node is visualized by an input field.
   */
  public nodeIsCheckbox = (node: ConfigNode) => node.type?.isNative && node.type?.typeName === 'System.Boolean' &&
    node.value !== null;

  /**
   * Returns if this node is set to null.
   */
  public nodeIsNull = (node: ConfigNode) => node.value === null;

  /**
   * Returns if this node is visualized by a drop-down box.
   */
  public nodeIsEnum = (node: ConfigNode) => node.type?.isEnum && node.value !== null;

  /**
   * Returns the input type for the given node.
   */
  public nodeGetInputType(node: ConfigNode) {
    switch (node.type?.typeName) {
      case 'System.String':
        return 'text';
      case 'System.Int32':
      case 'System.UInt32':
      case 'System.Int64':
      case 'System.UInt64':
        return 'number';
    }
    return 'text';
  }

  /**
   * Refreshes the tree view.
   */
  private updateTree() {
    const data = this.dataSource.data;
    this.dataSource.data = [];
    this.dataSource.data = data;
  }


  /**
   * Creates and adds a new node to the given list.
   */
  public onAddNodeClicked(node: ConfigNode) {
    if (!node.type) return;

    const elementType = this.types?.getListElementType(node.type);

    // TODO: Select type

    const instance = this.types?.createInstance(elementType);
    const child = this.buildNode(instance, elementType?.typeName, node);
    node.children?.push(child);

    this.updateTree();
  }

  /**
   * Removes the node from the parent list.
   */
  public onRemoveNodeClicked(node: ConfigNode) {
    if (!node.parent?.children) return;
    const index = node.parent.children.indexOf(node);
    if (index < 0) return;
    node.parent.children.splice(index, 1);
    this.updateTree();
  }

  /**
   * Changes the type of a node by replacing it.
   */
  public onChangeTypeClicked(node: ConfigNode) {
    if (!this.types || !node.targetType) return;

    // TODO: Select type
    for (const type of this.types.getAssignableTypes(node.targetType)) {
      console.log(type.typeName);
    }
  }

  /**
   * Returns the value for the given property for this object.
   */
  public getPropertyValue(value: any, property: TypePropertyInfoDto): any {
    if (!value) return undefined;

    // The value still points to the root object, but we need to access the property node.
    for (let propertyName in value) {
      if (propertyName.startsWith('$')) {
        value = value[propertyName];
        break;
      }
    }

    // The property is the only property and its values is stored directly into the content.
    if (property.isRootProperty)
      return value;

    // Gets the property
    for (let propertyName in value) {
      if (propertyName.localeCompare(property.name, undefined, { sensitivity: 'accent' }) === 0) {
        return value[propertyName];
      }
    }

    return undefined;
  }

  /**
   * Gets the display name for the given type.
   */
  public getDisplayNameFromType(type?: TypeInfoDto): string {
    if (!type) {
      return '-/-';
    }

    if (type.expressionName) {
      return type.expressionName;
    }

    let name = type.typeName;
    const index = name.lastIndexOf('.');
    if (index) {
      name = name.substring(index + 1);
    }
    return name;
  }
}
