import {TypePropertyInfoDto} from "./type-property-info-dto";

export interface TypeInfoDto {
  typeName: string,
  expressionName?: string,
  documentationXml?: string,
  isObject: boolean,
  isAbstract: boolean,
  isNative: boolean,
  isList: boolean,
  isEnum: boolean,
  parentTypeNames: string[],
  properties: TypePropertyInfoDto[],
  values: string[],
}
