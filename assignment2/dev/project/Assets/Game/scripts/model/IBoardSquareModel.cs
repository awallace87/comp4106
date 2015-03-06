using UnityEngine;
using System.Collections;

public interface IBoardSquareModel {
	uint MobilityScore { get; set; }

	bool ContainsDisc();
	IDiscModel Disc { get; set; }
}
