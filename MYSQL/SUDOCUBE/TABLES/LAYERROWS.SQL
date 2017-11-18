-- -----------------------------------------------------
-- Table `SUDOCUBE`.`LAYERROWS`
-- -----------------------------------------------------
DROP TABLE IF EXISTS LAYERROWS ;

CREATE TABLE LAYERROWS (
  layerRowID INT NOT NULL,  
  layerID INT NOT NULL,
  v0 TINYINT NOT NULL,
  v1 TINYINT NOT NULL,
  v2 TINYINT NOT NULL,
  v3 TINYINT NOT NULL,
  v4 TINYINT NOT NULL,
  v5 TINYINT NOT NULL,
  v6 TINYINT NOT NULL,
  v7 TINYINT NOT NULL,
  v8 TINYINT NOT NULL,
  created TIMESTAMP NOT NULL,
  PRIMARY KEY(layerRowID, layerID),
  CONSTRAINT fk_ROWS_LAYERS1
    FOREIGN KEY (layerID)
    REFERENCES LAYERS (layerID)
    ON DELETE CASCADE
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

SHOW WARNINGS;
