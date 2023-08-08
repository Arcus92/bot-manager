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
  public value?: any[];

  /**
   * The type of the array elements.
   */
  @Input()
  public targetType: TypeInfoDto | undefined | null;

  /**
   * User action to add a new item to this list.
   */
  public clickedAddItem() {
    if (!this.value) return;

    // The target type is defined.
    if (this.targetType && !this.targetType.isAbstract) {
      const item = this.types?.createInstance(this.targetType);
      this.value.push(item);
    } else {
      // TODO: Type select
    }
  }

  /**
   * User action to remove the n th item from this list.
   */
  public clickedRemoveItem(index: number) {
    if (!this.value) return;
    this.value.splice(index, 1);
  }

  /**
   * User action to replace an item in this list.
   */
  public clickedSetItem(index: number) {
    if (!this.value) return;
    // TODO: Item replace
  }

}
