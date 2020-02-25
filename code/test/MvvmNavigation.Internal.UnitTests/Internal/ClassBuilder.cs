using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Egor92.MvvmNavigation.Internal.UnitTests.Internal
{
    internal class ClassBuilder
    {
        private bool _toConfigureNavigationManagerAttribute;
        private Type _frameControlType;

        private ClassBuilder()
        {
        }

        internal static ClassBuilder CreateNewClass()
        {
            return new ClassBuilder();
        }

        internal ClassBuilder WithNavigationManagerAttribute(Type frameControlType)
        {
            _toConfigureNavigationManagerAttribute = true;
            _frameControlType = frameControlType;
            return this;
        }

        internal Type Build()
        {
            var typeBuilder = CreateTypeBuilder();
            CreateConstructor(typeBuilder);
            if (_toConfigureNavigationManagerAttribute)
            {
                ConfigureNavigationManagerAttribute(typeBuilder);
            }

            return typeBuilder.CreateType();
        }

        private static TypeBuilder CreateTypeBuilder()
        {
            var assemblyName = new AssemblyName($"GeneratedAssembly_{Guid.NewGuid():N}");
            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule($"GeneratedModule_{Guid.NewGuid():N}");
            return moduleBuilder.DefineType($"GeneratedModuleNavigationManager_{Guid.NewGuid():N}",
                                            TypeAttributes.Public
                                            | TypeAttributes.Class
                                            | TypeAttributes.AutoClass
                                            | TypeAttributes.AnsiClass
                                            | TypeAttributes.BeforeFieldInit
                                            | TypeAttributes.AutoLayout, null);
        }

        private void ConfigureNavigationManagerAttribute(TypeBuilder typeBuilder)
        {
            var types = new[]
            {
                typeof(Type)
            };
            var constructorValues = new object[]
            {
                _frameControlType
            };
            var constructorInfo =
                typeof(NavigationManagerAttribute).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
            var customAttributeBuilder = new CustomAttributeBuilder(constructorInfo, constructorValues);
            typeBuilder.SetCustomAttribute(customAttributeBuilder);
        }

        private static void CreateConstructor(TypeBuilder typeBuilder)
        {
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
        }
    }
}
