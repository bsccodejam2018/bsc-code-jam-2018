package town
import horse.Horse

case class Town (val index: Int, val horses: List[Horse]){ 
}

object Town {
  def fromString(i: Int)(s: String): Town = {
    new Town(i, List(Horse.fromString(i)(s)))
  }
}