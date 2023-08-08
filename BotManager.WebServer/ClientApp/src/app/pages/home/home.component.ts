import {Component, OnInit, ViewChild} from '@angular/core';
import {TypesService} from "../../services/types.service";
import {TypeInfoDto} from "../../dto/type-info-dto";
import {ConfigService} from "../../services/config.service";
import {TypeDefinitions} from "../../controllers/type-definitions";
import {TypeObjectComponent} from "../../components/type-object/type-object.component";
import {TypeEditorComponent} from "../../components/type-editor/type-editor.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(private configService: ConfigService, private typesService: TypesService) { }

  /**
   * Reference to the type editor.
   */
  @ViewChild('typeEditor') typeEditor?: TypeEditorComponent;

  /**
   * The type definitions.
   */
  public types?: TypeDefinitions;

  /**
   * The loaded config.
   */
  public config?: any;


  ngOnInit(): void {
    this.fetchConfig();
    this.fetchTypes();
  }

  private fetchConfig() {
    this.configService.get().subscribe(config => {
      this.config = config;
    });
  }

  private fetchTypes() {
    this.typesService.get().subscribe(types => {
      this.types = new TypeDefinitions(types);
    });
  }


  public onBuild() {
    const config = this.typeEditor?.value;
    alert(JSON.stringify(config));
  }

}
