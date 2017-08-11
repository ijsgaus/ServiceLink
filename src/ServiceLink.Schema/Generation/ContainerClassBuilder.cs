using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;

namespace ServiceLink.Schema.Generation
{
    public class ContainerClassBuilder
    {
        private readonly IEnumerable<Type> _types;
        private readonly string _nameSpace;

        public ContainerClassBuilder(IEnumerable<Type> types, string nameSpace)
        {
            _types = types;
            _nameSpace = nameSpace;
        }

        public Type GenerateContainerType()
        {
            var typeBuilder = CreateTypeBuilder();
            //var constructor =
                typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName |
                                                     MethodAttributes.SpecialName);
            foreach (var type in _types)
            {
                CreateProperty(typeBuilder, type.Name, type);
            }
            return typeBuilder.CreateTypeInfo().AsType();
        }

        private TypeBuilder CreateTypeBuilder()
        {
            var assemblyName = new AssemblyName(_nameSpace);
            var builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var module = builder.DefineDynamicModule("MainModule");
            var typeBuilder = module.DefineType("Container",
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit);
            return typeBuilder;
        }

        private static void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            var fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var propertyBuilder =
                typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            var requiredConstructor = typeof(RequiredAttribute).GetConstructor(new Type[0]);
            propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(requiredConstructor, new object[0]));
            var getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyName,
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType,
                Type.EmptyTypes);
            var getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setMethodBuilder = typeBuilder.DefineMethod("set_" + propertyName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig,
                null, new[] { propertyType });
            var setIl = setMethodBuilder.GetILGenerator();
            var modifyProperty = setIl.DefineLabel();
            var exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }
    }
}