﻿/* Dexer Copyright (c) 2010-2016 Sebastien LEBRETON

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. */

using System;
using System.Collections;
using System.Collections.Generic;
using Dexer.Core;
using Dexer.Instructions;

namespace Dexer.IO.Collectors
{
	internal class BaseCollector<T>
	{
		public Dictionary<T, int> Items { get; set; }

		public BaseCollector()
		{
			Items = new Dictionary<T, int>();
		}

		public List<T> ToList()
		{
			return new List<T>(Items.Keys);
		}

		public List<T> ToList(IComparer<T> comparer)
		{
			var list = ToList();
			list.Sort(comparer);
			return list;
		}

		public virtual void Collect(Dex dex)
		{
			Collect(dex.Classes);
		}

		public virtual void Collect(List<ClassDefinition> classes)
		{
			foreach (var @class in classes)
				Collect(@class);
		}

		public virtual void Collect(List<ClassReference> classes)
		{
			foreach (var @class in classes)
				Collect(@class);
		}

		public virtual void Collect(List<MethodDefinition> methods)
		{
			foreach (var method in methods)
				Collect(method);
		}

		public virtual void Collect(List<MethodReference> methods)
		{
			foreach (var method in methods)
				Collect(method);
		}

		public virtual void Collect(List<FieldDefinition> fields)
		{
			foreach (var field in fields)
				Collect(field);
		}

		public virtual void Collect(List<FieldReference> fields)
		{
			foreach (var field in fields)
				Collect(field);
		}

		public virtual void Collect(List<TypeReference> types)
		{
			foreach (var type in types)
				Collect(type);
		}

		public virtual void Collect(List<Annotation> annotations)
		{
			foreach (var annotation in annotations)
				Collect(annotation);
		}

		public virtual void Collect(List<Parameter> parameters)
		{
			foreach (var parameter in parameters)
				Collect(parameter);
		}

		public virtual void Collect(List<AnnotationArgument> arguments)
		{
			foreach (var argument in arguments)
				Collect(argument);
		}

		public virtual void Collect(List<Instruction> instructions)
		{
			foreach (var instruction in instructions)
				Collect(instruction);
		}

		public virtual void Collect(List<DebugInstruction> instructions)
		{
			foreach (var instruction in instructions)
				Collect(instruction);
		}

		public virtual void Collect(List<ExceptionHandler> exceptions)
		{
			foreach (var exception in exceptions)
				Collect(exception);
		}

		public virtual void Collect(List<Catch> catches)
		{
			foreach (var @catch in catches)
				Collect(@catch);
		}

		public virtual void Collect(ClassDefinition @class)
		{
			Collect(@class.Annotations);
			Collect(@class.Fields);
			Collect(@class.InnerClasses);
			Collect(@class.Interfaces);
			Collect(@class.Methods);
			Collect(@class.SourceFile);
			Collect(@class.SuperClass);
			Collect(@class as ClassReference);
		}

		public virtual void Collect(ClassReference @class)
		{
			Collect(@class as TypeReference);
		}

		public virtual void Collect(TypeReference tref)
		{
		}

		public virtual void Collect(MethodDefinition method)
		{
			Collect(method.Annotations);
			if (method.Body != null)
				Collect(method.Body);
			Collect(method as MethodReference);
		}

		public virtual void Collect(MethodReference method)
		{
			Collect(method.Prototype);
			Collect(method as IMemberReference);
		}

		public virtual void Collect(Prototype prototype)
		{
			Collect(prototype.Parameters);
			Collect(prototype.ReturnType);
		}

		public virtual void Collect(Parameter parameter)
		{
			Collect(parameter.Annotations);
			Collect(parameter.Type);
		}

		public virtual void Collect(FieldDefinition field)
		{
			Collect(field.Annotations);
			Collect(field.Value);
			Collect(field as FieldReference);
		}

		public virtual void Collect(FieldReference field)
		{
			Collect(field.Type);
			Collect(field as IMemberReference);
		}

		public virtual void Collect(IMemberReference member)
		{
			Collect(member.Name);

			var fieldReference = member as FieldReference;
			if (fieldReference != null)
				Collect(fieldReference.Owner);

			var methodReference = member as MethodReference;
			if (methodReference != null)
				Collect(methodReference.Owner);
		}

		public virtual void Collect(ArrayType array)
		{
			Collect(array as TypeReference);
			Collect(array.ElementType);
		}

		public virtual void Collect(CompositeType composite)
		{
			var classReference = composite as ClassReference;
			if (classReference != null)
				Collect(classReference);

			var arrayType = composite as ArrayType;
			if (arrayType != null)
				Collect(arrayType);
		}

		public virtual void Collect(Annotation annotation)
		{
			Collect(annotation.Arguments);
			Collect(annotation.Type);
		}

		public virtual void Collect(AnnotationArgument argument)
		{
			Collect(argument.Name);
			Collect(argument.Value);
		}

		public virtual void Collect(MethodBody body)
		{
			if (body.DebugInfo != null)
				Collect(body.DebugInfo);

			Collect(body.Instructions);
			Collect(body.Exceptions);
		}

		public virtual void Collect(Catch @catch)
		{
			Collect(@catch.Type);
		}

		public virtual void Collect(ExceptionHandler exception)
		{
			Collect(exception.Catches);
		}

		public virtual void Collect(Instruction instruction)
		{
			Collect(instruction.Operand);
		}

		public virtual void Collect(DebugInfo debugInfo)
		{
			Collect(debugInfo.DebugInstructions);
			Collect(debugInfo.Parameters);
		}

		public virtual void Collect(DebugInstruction instruction)
		{
			Collect(instruction.Operands);
		}

		public virtual void Collect(object obj)
		{
			if (obj == null)
				return;

			if (obj is CompositeType)
				Collect(obj as CompositeType);
			else if (obj is FieldReference)
				Collect(obj as FieldReference);
			else if (obj is MethodReference)
				Collect(obj as MethodReference);
			else if (obj is TypeReference)
				Collect(obj as TypeReference);
			else if (obj is Annotation)
				Collect(obj as Annotation);

			else if (obj is string)
				Collect(obj as string);

			else if (obj is IEnumerable)
				foreach (var iobj in (obj as IEnumerable))
					Collect(iobj);

			else if (obj is ValueType) return;
			else if (obj is Register) return;
			else if (obj is Instruction) return;
			else if (obj is PackedSwitchData) return;
			else if (obj is SparseSwitchData) return;
			else throw new ArgumentException(obj.GetType().Name);
		}

		public virtual void Collect(string str)
		{
		}

	}
}
