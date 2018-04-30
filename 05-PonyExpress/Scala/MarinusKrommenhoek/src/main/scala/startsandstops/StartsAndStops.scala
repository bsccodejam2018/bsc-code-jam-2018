package startsandstops

//Learned I code use package scope
class StartsAndStops (val list: List[StartAndStop]){
  
}

object StartsAndStops {
  def fromStream(stream: Stream[String]): StartsAndStops = {
    val list = stream.map(s => StartAndStop.fromString(s)).toList
    new StartsAndStops(list)
  }
}