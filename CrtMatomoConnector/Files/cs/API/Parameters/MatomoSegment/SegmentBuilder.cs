namespace MatomoConnector.API
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Builds different segments using common types.
	/// </summary>
	public class SegmentBuilder {

		private static IEnumerable<List<T>> SplitList<T>(List<T> sourceList, int batchSize) {
			for (var i = 0; i < sourceList.Count; i += batchSize) {
				yield return sourceList.GetRange(i, Math.Min(batchSize, sourceList.Count - i));
			}
		}

		/// <summary>
		/// Returns a chain of segments by single column connected with <paramref name="condition"/>.
		/// </summary>
		public static IEnumerable<MatomoSegment> Create(string column, SegmentOperator segmentOperator,
				IEnumerable<string> values, SegmentCondition condition = SegmentCondition.OR, int batchSize = 30) {
			if (values == default || !values.Any()) {
				return new[] { new MatomoSegment(column, segmentOperator, string.Empty) };
			}
			var result = new List<MatomoSegment>();
			var batches = condition == SegmentCondition.AND
				? Enumerable.Repeat(values, 1)
				: SplitList(values.ToList(), batchSize);
			foreach (var valuesBatch in batches) {
				var prevSegment = default(MatomoSegment);
				var initialSegment = default(MatomoSegment);
				foreach (var value in valuesBatch) {
					var segment = new MatomoSegment(column, segmentOperator, value);
					if (prevSegment == default) {
						prevSegment = segment;
						initialSegment = segment;
						continue;
					}
					prevSegment = prevSegment.AddCondition(condition, segment);
				}
				result.Add(initialSegment);
			}
			return result;
		}
		
	}
}
