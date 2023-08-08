import {Component, Input} from '@angular/core';
import {ConfigTypes} from "../../controllers/config-types";
import {TypeInfoDto} from "../../dto/type-info-dto";

/**
 * This component is the editor for type arrays or lists.
 */
@Component({
  selector: 'app-config-list',
  templateUrl: './config-list.component.html',
  styleUrls: ['../config-editor/config-editor.component.css']
})
export class ConfigListComponent {
  constructor() { }

  /**
   * The type definitions.
   */
  @Input()
  public types?: ConfigTypes;

  /**
   * The list / array.
   */
  @Input()
  public value: any;

  /**
   * The type of the array elements.
   */
  @Input()
  public targetType: TypeInfoDto | undefined | null;

}
