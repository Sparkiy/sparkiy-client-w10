using Microsoft.Practices.Unity;

namespace sparkiy.DI
{
	/// <summary>
	/// DI container.
	/// </summary>
	public static class Container
	{
		/// <summary>
		/// Initializes container.
		/// </summary>
		public static void Initialize()
		{
			Instance = new UnityContainer();
		}


		/// <summary>
		/// The instance.
		/// </summary>
		public static UnityContainer Instance { get; private set; }
	}
}
