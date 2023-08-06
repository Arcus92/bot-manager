import {Component, Input, OnInit} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {TypePropertyInfoDto} from "../../dto/type-property-info-dto";
import {TypeList} from "../../controllers/type-list";

@Component({
  selector: 'app-type-value',
  templateUrl: './type-value.component.html',
  styleUrls: ['./type-value.component.css']
})
export class TypeValueComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  public collapsed: boolean = false;

  @Input()
  public types?: TypeList;


  private _targetType: TypeInfoDto | undefined | null = undefined;
  @Input()
  public set targetType(value: TypeInfoDto | undefined | null) {
    this._targetType = value;
    this.updateValueType();
  }

  public get targetType(): TypeInfoDto | undefined | null {
    return this._targetType;
  }



  private _value: any;
  @Input()
  public set value(value: any) {
    this._value = value;
    this.updateValueType();
  }

  public get value(): any {
    return this._value;
  }

  public valueType: TypeInfoDto | null | undefined = null;

  private updateValueType() {
    this.valueType = this.types?.getTypeByObject(this._value, this.targetType);
  }

  public get valueContent(): any {
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

  public getPropertyValue(property: TypePropertyInfoDto): any {
    if (!this._value) return undefined;

    const content = this.valueContent;

    // The property is the only property and its values is stored directly into the content.
    if (property.isContentProperty)
      return content;

    // Gets the property
    for (let propertyName in content) {
      if (propertyName.localeCompare(property.name, undefined, { sensitivity: 'accent' }) === 0) {
        return content[propertyName];
      }
    }

    return undefined;
  }

  public getPropertyTargetType(property: TypePropertyInfoDto): TypeInfoDto | undefined {
    return this.types?.getTypeByTypeName(property.typeName);
  }

  public getNativeInputType(): string {
    if (this.valueType?.typeName == 'System.Boolean') {
      return 'checkbox';
    }

    if (this.valueType?.typeName == 'System.String') {
      return 'text';
    }
    return 'number';
  }
}
