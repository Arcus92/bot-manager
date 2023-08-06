import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {TypesService} from "../../services/types.service";
import {TypeInfoDto} from "../../dto/type-info-dto";
import {ConfigService} from "../../services/config.service";
import {TypePropertyInfoDto} from "../../dto/type-property-info-dto";
import {TypeList} from "../../controllers/type-list";
import {TypeValueComponent} from "../type-value/type-value.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  constructor(private configService: ConfigService, private typesService: TypesService) { }

  @ViewChild('rootNode') rootNode?: TypeValueComponent;

  /**
   * The loaded config.
   */
  private config?: any;

  public types?: TypeList;

  public rootType: TypeInfoDto | null | undefined;
  public rootValue?: any;

  ngOnInit(): void {
    this.fetchConfig();
    this.fetchTypes();
  }

  private fetchConfig() {
    this.configService.get().subscribe(config => {
      this.config = config;
      this.initBuilder();
    });
  }

  private fetchTypes() {
    this.typesService.get().subscribe(types => {
      this.types = new TypeList(types);
      this.initBuilder();
    });
  }


  private initBuilder() {
    if (!this.config || !this.types)
      return;

    this.rootType = this.types?.getTypeByExpressionName("$List");
    this.rootValue = this.config;
  }

  public onBuild() {
    console.log(this.rootNode);
    const config = this.rootNode?.value;
    alert(JSON.stringify(config));
  }

}
