package horses

class Horse (val endurance: Int, val speed: Int, val visitedTowns: Set[Int]){
  def ride(town: Int, distance: Int): (Horse, Float) = 
    (new Horse(this.endurance - distance, this.speed, this.visitedTowns + town), distance.toFloat / this.speed.toFloat) 
  def mkString: String = s"Horse( endurance=${this.endurance}, speed=${this.speed}, visitedTowns=${this.visitedTowns})"
  override def toString(): String = this.mkString
}

object Horse {
  def apply(endurance: Int, speed: Int, visitedTowns: Set[Int]) = new Horse(endurance, speed, visitedTowns)
  def fromString(town: Int)(s: String): Horse = {
    val data = s.split(" ").map(s => s.toInt).take(2)
    new Horse(data(0), data(1), Set(town)) 
  }
}