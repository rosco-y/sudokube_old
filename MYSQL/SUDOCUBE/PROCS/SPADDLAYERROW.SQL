DELIMITER $$

DROP PROCEDURE IF EXISTS `spAddLayerRow` $$

CREATE PROCEDURE `spAddLayerRow` (
  inLayerID INT,
  inLayerRowID INT,
  in0	TINYINT,
  in1	TINYINT,
  in2	TINYINT,
  in3	TINYINT,
  in4	TINYINT,
  in5	TINYINT,
  in6	TINYINT,
  in7	TINYINT,
  in8	TINYINT
  )
BEGIN
  INSERT INTO LAYERROWS(
    layerRowID,	
	layerID,
	v0,
	v1,
	v2,
	v3,
	v4,
	v5,
	v6,
	v7,
	v8
	)	
    VALUES(
    inLayerRowID,	
	inLayerID,
	in0,
	in1,
	in2,
	in3,
	in4,
	in5,
	in6,
	in7,
	in8
	);
END $$

DELIMITER ;
