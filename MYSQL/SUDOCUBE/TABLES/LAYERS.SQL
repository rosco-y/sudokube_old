-- -----------------------------------------------------
-- Table SUDOCUBE.LAYERS
-- -----------------------------------------------------
DROP TABLE IF EXISTS LAYERS;

CREATE TABLE LAYERS (
  puzzleID INT NOT NULL,
  layerID INT NOT NULL,
  created TIMESTAMP NOT NULL, 
  PRIMARY KEY(puzzleID, layerID),
  CONSTRAINT fk_LAYERS_PUZZLES
    FOREIGN KEY (puzzleID)
	REFERENCES PUZZLES (puzzleID)
		ON DELETE CASCADE
		ON UPDATE NO ACTION
)	
ENGINE = InnoDB;

SHOW WARNINGS;

