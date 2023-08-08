import {Component, OnInit, ViewChild} from '@angular/core';
import {TypesService} from "../../services/types.service";
import {ConfigService} from "../../services/config.service";
import {ConfigTypes} from "../../controllers/config-types";
import {ConfigEditorComponent} from "../../components/config-editor/config-editor.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(private configService: ConfigService, private typesService: TypesService) { }

  /**
   * Reference to the type editor.
   */
  @ViewChild('typeEditor') typeEditor?: ConfigEditorComponent;

  /**
   * The type definitions.
   */
  public types?: ConfigTypes;

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
      this.types = new ConfigTypes(types);
    });
  }


  public onBuild() {
    const config = this.typeEditor?.value;
    alert(JSON.stringify(config));
  }

}
