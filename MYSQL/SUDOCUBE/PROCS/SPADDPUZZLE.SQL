DELIMITER $$

DROP PROCEDURE IF EXISTS `spAddPuzzle` $$

CREATE PROCEDURE `spAddPuzzle` (
  inPID INT,
  inDescription VARCHAR(200),
  inTotalSeconds MEDIUMINT,
  inNumberOfValuesTested BIGINT,
  inSuccess TINYINT
  )
BEGIN
  INSERT INTO PUZZLES(
    puzzleID,
    description,
    totalSeconds,
    numberOfValuesTested,
    Success
	)	
    VALUES(
    inPID,
    inDescription,
    inTotalSeconds,
    inNumberofValuesTested,
    inSuccess
	);
END $$

DELIMITER ;
