-- -----------------------------------------------------
-- Table `SUDOCUBE`.`PUZZLES`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SUDOCUBE`.`PUZZLES` ;

CREATE TABLE PUZZLES (
  puzzleID INT NOT NULL PRIMARY KEY,
  description VARCHAR(200) NOT NULL,
  totalSeconds MEDIUMINT NOT NULL DEFAULT 0,
  numberOfValuesTested BIGINT UNSIGNED NOT NULL DEFAULT 0,
  Success TINYINT(1) NOT NULL DEFAULT 0,
  created TIMESTAMP NOT NULL)
ENGINE = InnoDB;

SHOW WARNINGS;
