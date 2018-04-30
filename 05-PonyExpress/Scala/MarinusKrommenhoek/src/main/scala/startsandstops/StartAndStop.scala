package startsandstops

class StartAndStop (val start: Int, val stop: Int){
  
}

object StartAndStop {
  def apply(start: Int, stop: Int): StartAndStop = new StartAndStop(start, stop)
  def fromString(s: String): StartAndStop = {
    val data = s.split(" ").map(s => s.toInt).take(2)
    new StartAndStop(data(0) - 1, data(1) - 1) // Offset to accommodate arrays
  }
}