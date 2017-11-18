DELIMITER $$

DROP PROCEDURE IF EXISTS `spAddLayer` $$

CREATE PROCEDURE `spAddLayer` (
  inPuzzleID INT,
  inLayerID INT
  )
BEGIN
  INSERT INTO LAYERS(
    layerID,
	puzzleID
	)	
    VALUES(
    inLayerID,
	inPuzzleID
	);
END $$

DELIMITER ;
