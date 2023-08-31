import {Component, Input} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {ConfigTypes} from "../../controllers/config-types";
import {NestedTreeControl} from "@angular/cdk/tree";
import {MatTreeNestedDataSource} from "@angular/material/tree";
import {TypePropertyInfoDto} from "../../dto/type-property-info-dto";

interface ConfigNode {
  name: string;
  value?: any;
  documentationXml?: string;
  hasInput: boolean;
  isArray: boolean;
  isArrayElement: boolean;
  children?: ConfigNode[];
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
    const node = this.buildNode(this._value, targetType?.typeName);
    this.dataSource.data = [node];
  }


  public root?: ConfigNode;
  treeControl = new NestedTreeControl<ConfigNode>(node => node.children);
  dataSource = new MatTreeNestedDataSource<ConfigNode>();

  private buildNode(value: any, targetTypeName?: string): ConfigNode {
    // This is an array
    let isArray = false;
    if (targetTypeName?.endsWith('[]')) {
      targetTypeName = targetTypeName.substring(0, targetTypeName?.length - 2);
      isArray = true;
    }

    const targetType = targetTypeName ? this.types?.getTypeByTypeName(targetTypeName) : undefined;
    const type = this.types?.getTypeByObject(value) ?? targetType;



    const node: ConfigNode = {
      name: type?.typeName ?? '-/-',
      value: value,
      hasInput: false,
      isArray: false,
      isArrayElement: false,
      documentationXml: type?.documentationXml,
    };

    if (!value) {
      node.name = '-/-';
    }

    if (type) {
      node.hasInput = type.isEnum || type.isNative;

      if (isArray) {
        node.name += '[]';
        node.children = [];
        node.isArray = true;
        if (value) {
          for (const child of value) {
            const childNode = this.buildNode(child, targetType?.typeName);
            childNode.isArrayElement = true;
            node.children.push(childNode);
          }
        }
      } else if (type.isList) {
        node.children = [];
        node.isArray = true;
        if (value) {
          for (const child of value) {
            const childNode = this.buildNode(child);
            childNode.isArrayElement = true;
            node.children.push(childNode);
          }
        }
      } else if (!type.isNative && !type.isEnum) {
        node.children = [];
        for (const property of type.properties) {
          const propertyValue = this.getPropertyValue(value, property);

          node.children.push({
            name: property.name,
            hasInput: false,
            isArray: false,
            isArrayElement: false,
            documentationXml: property.documentationXml,
            children: [this.buildNode(propertyValue, property.typeName)]
          });
        }
      }
    }

    return node;
  }

  /**
   * Returns if the given node has children nodes.
   */
  public hasChild = (_: number, node: ConfigNode) => !!node.children && node.children.length > 0;


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
}
