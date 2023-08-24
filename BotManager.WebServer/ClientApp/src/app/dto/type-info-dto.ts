import {TypePropertyInfoDto} from "./type-property-info-dto";

export interface TypeInfoDto {
    typeName: string,
    expressionName?: string,
    documentation?: string,
    isAbstract: boolean,
    isNative: boolean,
    isList: boolean,
    isEnum: boolean,
    parentTypeNames: string[],
    properties: TypePropertyInfoDto[],
    values: string[],
}
