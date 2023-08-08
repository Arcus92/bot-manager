import {Component, Input} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {TypeDefinitions} from "../../controllers/type-definitions";

@Component({
  selector: 'app-type-editor',
  templateUrl: './type-editor.component.html',
  styleUrls: ['./type-editor.component.css']
})
export class TypeEditorComponent {
  constructor() { }

  /**
  * The type definitions.
  */
  private _types: TypeDefinitions | undefined;

  @Input()
  public set types(value: TypeDefinitions | undefined) {
    this._types = value;
    this.updateTargetType();
  }
  public get types(): TypeDefinitions | undefined {
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
    this.targetType = this.types?.getTypeByExpressionName("$List");
  }


}
