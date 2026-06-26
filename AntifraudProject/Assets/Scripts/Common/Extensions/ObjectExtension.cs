using JetBrains.Annotations;
using UnityEngine;

namespace Common.Extensions
{
	public static class ObjectExtension
	{
		[ContractAnnotation("obj:null => true")]
		public static bool IsNull(this Object obj) => ReferenceEquals(obj, null);

		[ContractAnnotation("obj:null => false")]
		public static bool IsNotNull(this Object obj) => !obj.IsNull();

		public static void SuitableDestroy(this Object obj)
		{
			if (Application.isEditor)
				Object.DestroyImmediate(obj);
			else
				Object.Destroy(obj);
		}

		public static bool IsNullOrDestroyed(object obj)
		{
			return obj switch {
				null => true,
				Object o => o == null,
				_ => false
			};
		}
	}
}