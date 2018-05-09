package horse

case class Horse (val speed: Double, val endurance: Double, val visitedTowns: Set[Int]){
  def mkString: String = s"Horse( endurance=${this.endurance}, speed=${this.speed}, visitedTowns=${this.visitedTowns})"
  override def toString(): String = this.mkString
}

object Horse {
  def fromString(town: Int)(s: String): Horse = {
    val data = s.split(" ").map(s => s.toDouble).take(2)
    new Horse(data(1), data(0), Set(town)) 
  }
}