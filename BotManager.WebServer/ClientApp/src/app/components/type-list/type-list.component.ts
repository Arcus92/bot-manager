import {Component, Input} from '@angular/core';
import {TypeDefinitions} from "../../controllers/type-definitions";
import {TypeInfoDto} from "../../dto/type-info-dto";

/**
 * This component is the editor for type arrays or lists.
 */
@Component({
  selector: 'app-type-list',
  templateUrl: './type-list.component.html',
  styleUrls: ['./type-list.component.css']
})
export class TypeListComponent {
  constructor() { }

  /**
   * The type definitions.
   */
  @Input()
  public types?: TypeDefinitions;

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
