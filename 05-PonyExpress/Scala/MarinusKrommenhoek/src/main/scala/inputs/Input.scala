package inputs
import scala.io.Source.fromFile
import state.State
import town.Town
import horse.Horse
import rider.Rider

object Input {
  def input(filename: String): Iterator[String]= {
    fromFile(filename).getLines()
  }
  def parse(filename: String): Map[Int, Map[String, Stream[String]]] = {
    val _input = input(filename).toStream
    val cases = _input(0).toInt
    def _parse(i: Int, start: Int, stream: Stream[String], answer: Map[Int, Map[String, Stream[String]]] = Map.empty): Map[Int, Map[String, Stream[String]]] = {
      if (stream.isEmpty) 
      {answer}
      else if (i > cases) 
      {answer}
      else {
      val (n, q) = stream(start).split(" ").map(s => s.toInt).take(2) match {case Array(n: Int, q: Int) => (n, q)}
      val shorses = start + 1
      val fhorses = start + n
      val stowns = start +1 + n
      val ftowns = start + 2*n
      val sdestinations = start + 1 + 2*n
      val fdestinations = start + 2*n + q
      val nstart = start + 2*n + q + 1
      val horses = Map("horses" -> stream.slice(shorses, fhorses +1))
      val townchart = Map("map" -> stream.slice(stowns, ftowns + 1))
      val startAndStops = Map("riders" -> stream.slice(sdestinations, fdestinations + 1))
      _parse(i + 1, nstart, stream, answer  ++ Map(i -> (horses ++ townchart ++ startAndStops)))
      }
    }
    _parse(1, 1, _input)
    }
  def completeParse(filename: String): Map[Int, List[State]] = {
    val data = parse(filename)
    def towns(i: Int): Map[Int, List[Horse]] = {
      def horse(index: Int)(s: String): Horse = Horse.fromString(index)(s)
      data(i)("horses").
      zipWithIndex.
      map{case (value: String, index: Int) => (index, List(horse(index)(value)))}.
      toMap
    }
    def map(i: Int): List[List[Double]] = {
      data(i)("map").map(s => s.split(" ").toList.map(s => s.toDouble)).toList
    }
    def riders(i: Int): List[Rider] = {
      data(i)("riders").map(s => Rider.fromString(s)).toList
    }
    def states(i: Int): List[State] = {
      val m = map(i)
      val ts = towns(i)
      for( r <- riders(i)) yield new State(ts, m, r)
    }
    data.keys.map( i => (i,states(i))).toMap
  }
}