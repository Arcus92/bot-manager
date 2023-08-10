import {Component, Input} from '@angular/core';
import {TypeInfoDto} from "../../dto/type-info-dto";
import {ConfigTypes} from "../../controllers/config-types";
import {NestedTreeControl} from "@angular/cdk/tree";
import {MatTreeNestedDataSource} from "@angular/material/tree";

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

    this.targetType = this.types?.getTypeByExpressionName("$List");
  }
}
