namespace Creatio.ContentDesigner
{
	internal class ResponseResult<T>
	{
		public T Content { get; set; }
		public bool Success { get; set; }
		public string Error { get; set; }

	}
}
