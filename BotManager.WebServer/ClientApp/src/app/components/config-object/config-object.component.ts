import {Component, Input} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {TypePropertyInfoDto} from "../../dto/type-property-info-dto";
import {ConfigTypes} from "../../controllers/config-types";
import {FormControl} from "@angular/forms";

/**
 * This component is the editor for an type object.
 */
@Component({
  selector: 'app-config-object',
  templateUrl: './config-object.component.html',
  styleUrls: ['../config-editor/config-editor.component.css']
})
export class ConfigObjectComponent {

  constructor() { }

  formControl = new FormControl('TEST', []);

  /**
   * The type definitions.
   */
  @Input()
  public types?: ConfigTypes;

  /**
   * The target type of the value. The value must match this type of a any type inherent from this.
   */
  private _targetType: TypeInfoDto | undefined | null = undefined;

  @Input()
  public set targetType(value: TypeInfoDto | undefined | null) {
    this._targetType = value;
    this.updateValueType();
  }
  public get targetType(): TypeInfoDto | undefined | null {
    return this._targetType;
  }


  /**
   * The value to edit.
   */
  private _value: any;
  @Input()
  public set value(value: any) {
    this._value = value;
    this.updateValueType();
  }

  public get value(): any {
    return this._value;
  }

  /**
   * The current type of the value.
   */
  public valueType: TypeInfoDto | null | undefined = null;

  /**
   * Fetches the actually value type from the given value object.
   */
  private updateValueType() {
    this.valueType = this.types?.getTypeByObject(this._value, this.targetType);
  }

  /**
   * Returns the json node that holds the actually values / properties.
   * Expressions values have this upper type layer with the $typeName property. This will always link to the sub-element
   * for expressions. For other value types, this will link to the same node as value does.
   */
  public get valueNode(): any {
    if (!this._value) return undefined;

    // Only expressions have this $ type wrapper. All other types must be clearly defined.
    if (!this.types?.isExpressionType(this.valueType)) {
      return this._value;
    }

    for (let propertyName in this._value) {
      if (propertyName.startsWith('$')) {
        return this._value[propertyName];
      }
    }
    return undefined;
  }

  public set valueNode(value: any) {
    // Only expressions have this $ type wrapper. All other types must be clearly defined.
    if (!this.types?.isExpressionType(this.valueType)) {
      this._value = value;
      return;
    }

    for (let propertyName in this._value) {
      if (propertyName.startsWith('$')) {
        this._value[propertyName] = value;
        return;
      }
    }
  }

  /**
   * Returns the value for the given property for this object.
   */
  public getPropertyValue(property: TypePropertyInfoDto): any {
    if (!this._value) return undefined;

    const content = this.valueNode;

    // The property is the only property and its values is stored directly into the content.
    if (property.isRootProperty)
      return content;

    // Gets the property
    for (let propertyName in content) {
      if (propertyName.localeCompare(property.name, undefined, { sensitivity: 'accent' }) === 0) {
        return content[propertyName];
      }
    }

    return undefined;
  }

  /**
   * Sets the value for the given property for this object.
   */
  public setPropertyValue(property: TypePropertyInfoDto, value: any) {
    if (!this._value) return;

    const content = this.valueNode;

    // The property is the only property and its values is stored directly into the content.
    if (property.isRootProperty) {
      this.valueNode = value;
      return;
    }

    // Gets the property
    for (let propertyName in content) {
      if (propertyName.localeCompare(property.name, undefined, { sensitivity: 'accent' }) === 0) {
        content[propertyName] = value;
      }
    }
  }

  /**
   * Returns the target type for the given property in this object.
   */
  public getPropertyTargetType(property: TypePropertyInfoDto): TypeInfoDto | undefined {
    return this.types?.getTypeByTypeName(property.typeName);
  }

  /**
   * Helper to get the correct input type for a native field.
   */
  public getNativeInputType(): string {
    if (this.valueType?.typeName == 'System.Boolean') {
      return 'checkbox';
    }

    if (this.valueType?.typeName == 'System.String') {
      return 'text';
    }
    return 'number';
  }

  /**
   * User action to replace an item in this object.
   */
  public clickedSetItem(property: TypePropertyInfoDto) {
    if (!this.value) return;
    const propertyType = this.types?.getTypeByTypeName(property.typeName);
    console.log(propertyType);
    if (propertyType && !propertyType.isAbstract) {
      const item = this.types?.createInstance(propertyType);
      this.setPropertyValue(property, item);
    } else {
      // TODO: Type select
    }
    // TODO: Item replace
  }
}
