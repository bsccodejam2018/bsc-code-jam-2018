package towns
import horses.Horse

class Town (val index: Int){
  var horses = List.empty[Horse]
  def add(horse: Horse) = {
    val _horses = horse +: this.horses
    val _nTown = new Town(this.index)
    _nTown.horses = _horses
    _nTown
  }
  def remove(horse: Horse): Town = {
    val _horses = this.horses.filter(hrse => hrse != horse)
    val _nTown = new Town(this.index)
    _nTown.horses = _horses
    _nTown
  }
}

object Town {
  def apply(index: Int): Town = new Town(index)
}

class Towns (val towns: Map[Int, Town]){
}

object Towns {
  def fromStream(stream: Stream[String]): Towns = {
    val map = stream.
              zipWithIndex.
              map{case (s:String, index: Int) => Map(index -> Town(index).add(Horse.fromString(index)(s)))}.
              reduce( (m, n) => m ++ n)
    new Towns(map)
  }
}

class TownsChart {
  var chart: Array[Array[Int]] = Array.empty[Array[Int]]
}

object TownsChart {
  def fromStream(chart: Stream[String]): TownsChart = {
    val map = new TownsChart()
    map.chart = chart.map(line => line.split(" ").map(s => s.toInt)).toArray
    map
  }
}