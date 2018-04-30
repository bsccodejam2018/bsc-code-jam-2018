package algorithm
import towns.{Town, Towns, TownsChart}
import startsandstops.{StartsAndStops}
import horses.Horse
import scala.math.Ordering

class Algorithm (val scenario: Map[String, Stream[String]]){
  val townChart = TownsChart.fromStream(scenario("townchart"))
  val startsAndStops = StartsAndStops.fromStream(scenario("startAndStops"))
  val towns = Towns.fromStream(scenario("horses"))
  
  def availableHorses(town: Town): List[Horse] = {
    town.horses
  }
  
  def reachableTowns(currentTown: Int)(horse: Horse): List[Int] = {
    townChart.
    chart(currentTown).
    zipWithIndex.
    map{case (distance: Int, townIndex: Int) => (townIndex, distance)}.
    filter{case (townIndex: Int, distance: Int) => distance >= 0}.
    map{case (townIndex: Int, distance: Int) => townIndex}.
    filter(index => horse.endurance >= townChart.chart(currentTown)(index) && horse.endurance >= 0).
    filter(index => !horse.visitedTowns.contains(index)).
    toList
  }
  
  def updateTowns(towns: Towns, time: Float)(horse: Horse)(currentTown: Int)(destinationTown: Int): (Towns, Float) = {
    val _towns = new Towns(towns.towns)
    _towns.towns(currentTown).remove(horse)
    val (nhorse, timeTaken) = horse.ride(destinationTown, this.townChart.chart(currentTown)(destinationTown))
    _towns.towns(destinationTown).add(nhorse)
    (_towns, time + timeTaken)
  }
  
   
  def fsolve(towns: Towns, currentTown: Int, destination: Int, time: Float =  0): Option[Float]  = {
    def _solve(time: Float, towns: Towns, currentTown: Int, destination: Int): Option[Float] = {
      if (currentTown == destination){
        Some(time)
        } else {
          val ls = for{h <- this.availableHorses(towns.towns(currentTown)); 
                       t <- this.reachableTowns(currentTown)(h)
                       } yield {
                         val (twns: Towns, tme: Float) = this.updateTowns(towns, time)(h)(currentTown)(t)
                         _solve(tme, twns,t, destination)
                       }
         ls.filter(_.isDefined) match {
           case l: List[Option[Float]] => if(l.isEmpty) None else l.min
         }            
       } 
      }
    _solve(time, towns, currentTown, destination)
  }
  
  def output: String = {
    this.startsAndStops.list.map(s => this.fsolve(this.towns, s.start, s.stop) match {
      case Some(time: Float) => time.toString()
      case _ => ""
    }).mkString(" ")
  }
  
}