package rider

case class Rider (val c: Int, val dest: Int){  
}

object Rider {
  def fromString(s: String): Rider = {
    val data = s.split(" ").map(s => s.toInt).take(2)
    new Rider(data(0) - 1, data(1) - 1)
  }
}