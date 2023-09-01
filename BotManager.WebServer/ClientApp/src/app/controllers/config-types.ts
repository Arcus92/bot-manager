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

    // Non-expression types
    if (targetType && !this.isExpressionType(targetType)) {
      return targetType;
    }

    // List
    if (Array.isArray(object)) {
      return this.getTypeByExpressionName("$List");
    }

    // Expression
    for (let propertyName in object) {
      if (propertyName.startsWith('$')) {
        return this.getTypeByExpressionName(propertyName);
      }
    }

    return undefined;
  }

  /**
   * Returns true if the given target type can be assigned by the given type.
   */
  public isAssignableFrom(targetType: TypeInfoDto, type: TypeInfoDto): boolean {
    // Check exact type
    if (targetType.typeName === type.typeName)
      return true;

    // Check parent types
    for (const parentTypeName of type.parentTypeNames) {
      // TODO: May support multiple levels of inheritance
      if (targetType.typeName === parentTypeName)
        return true;
    }

    return false;
  }

  /**
   * Returns a list of types that are assignable by the given target type.
   */
  public getAssignableTypes(targetType: TypeInfoDto, includeAbstract = false): TypeInfoDto[] {
    const list: TypeInfoDto[] = [];
    for (const type of this.types) {
      if (type.isAbstract && !includeAbstract) continue;
      if (this.isAssignableFrom(targetType, type)) {
        list.push(type);
      }
    }
    return list;
  }

  /**
   * Returns if the given type is an IExpression.
   */
  public isExpressionType(type: TypeInfoDto | null | undefined): boolean {
    if (type?.typeName === 'BotManager.Runtime.IExpression')
      return true;

    return (type?.parentTypeNames?.indexOf('BotManager.Runtime.IExpression') ?? -1) >= 0;
  }

  /**
   * Creates an empty instance from the given type.
   */
  public createInstance(type?: TypeInfoDto): any {
    if (!type || type.isAbstract) return null;

    // Handle native types
    switch (type.typeName) {
      case 'System.String':
        return '';
      case 'System.Int32':
      case 'System.UInt32':
      case 'System.Int64':
      case 'System.UInt64':
        return 0;
      case 'System.Boolean':
        return false;
    }

    // Handle enums
    if (type.isEnum && type.values.length > 0) {
      return type.values[0];
    }

    // Handle lists
    if (type.isList) {
      return [];
    }

    // Handle objects
    let properties: any = {};
    let instance: any = properties;

    // Add properties
    for (const property of type.properties) {
      const propertyType = this.getTypeByTypeName(property.typeName);
      const value = this.createInstance(propertyType);
      if (property.isRootProperty) {
        properties = value;
        break;
      }

      properties[property.name] = value;
    }

    // Expression wrapper
    if (type.expressionName) {
      instance = {};
      instance[type.expressionName] = properties;
    }

    return instance;
  }

  /**
   * Returns the element type of a list or array type.
   */
  public getListElementType(type: TypeInfoDto): TypeInfoDto | undefined {
    // Typed array
    if (type.typeName.endsWith('[]')) {
      const typeName = type.typeName.substring(0, type.typeName.length - 2);
      return this.getTypeByTypeName(typeName);
    }

    // Normal expression list
    return this.getTypeByTypeName('BotManager.Runtime.IExpression');
  }
}
