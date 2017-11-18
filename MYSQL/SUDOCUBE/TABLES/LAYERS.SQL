-- -----------------------------------------------------
-- Table SUDOCUBE.LAYERS
-- -----------------------------------------------------
DROP TABLE IF EXISTS LAYERS;

CREATE TABLE LAYERS (
  layerID INT NOT NULL,
  puzzleID INT NULL,
  created TIMESTAMP NOT NULL, 
  PRIMARY KEY(layerID, puzzleID),
  CONSTRAINT fk_LAYERS_PUZZLES
    FOREIGN KEY (puzzleID)
	REFERENCES PUZZLES (puzzleID)
		ON DELETE CASCADE
		ON UPDATE NO ACTION)	
ENGINE = InnoDB;

SHOW WARNINGS;

