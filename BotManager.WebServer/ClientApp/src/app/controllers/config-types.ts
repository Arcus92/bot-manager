import {TypeInfoDto} from "../dto/type-info-dto";

/**
 * This object holds a list of all types.
 * It provides methods to find a type by name, expression name or the object itself.
 */
export class ConfigTypes {
  public constructor(types: TypeInfoDto[]) {
    this.types = types;
  }

  /**
   * All registered types.
   */
  private readonly types: TypeInfoDto[] = [];

  /**
   * Returns the type (if found) by the full type name.
   */
  public getTypeByTypeName(typeName: string): TypeInfoDto | undefined {
    for (let type of this.types) {
      if (type.typeName == typeName)
        return type;
    }

    return undefined;
  }

  /**
   * Returns the type (if found) by the expression name - including the $.
   */
  public getTypeByExpressionName(expressionName: string): TypeInfoDto | undefined {
    for (let type of this.types) {
      if (type.expressionName == expressionName)
        return type;
    }

    return undefined;
  }

  /**
   * Returns the type of the given value object. The expected target type should be provided as a hint, if multiple
   * types are possible.
   */
  public getTypeByObject(object: any, targetType?: TypeInfoDto | null): TypeInfoDto | null | undefined {
    // Null
    if (object == null) {
      return null;
    }

    // String
    if (typeof object === 'string' || object instanceof String) {
      if (targetType?.isEnum) {
        return targetType;
      }

      return this.getTypeByTypeName("System.String");
    }

    // Number
    if (typeof object === 'number' || object instanceof Number) {
      if (targetType?.isNative) {
        return targetType;
      }

      return this.getTypeByTypeName("System.Int32");
    }

    // Boolean
    if (object === true || object === true) {
      return this.getTypeByTypeName("System.Boolean");
    }

    // List
    if (Array.isArray(object)) {
      return this.getTypeByExpressionName("$List");
    }

    // Non-expression types
    if (targetType && !this.isExpressionType(targetType)) {
      return targetType;
    }

    // Expression
    let expressionName: string | null = null;
    for (let propertyName in object) {
      if (propertyName.startsWith('$')) {
        return this.getTypeByExpressionName(propertyName);
      }
    }

    return undefined;
  }

  /**
   * Returns if the given type is an IExpression.
   */
  public isExpressionType(type: TypeInfoDto | null | undefined): boolean {
    if (type?.typeName === 'BotManager.Runtime.IExpression')
      return true;

    return (type?.parentTypeNames?.indexOf('BotManager.Runtime.IExpression') ?? -1) >= 0;
  }
}
