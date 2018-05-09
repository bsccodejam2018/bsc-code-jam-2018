package state
import horse.Horse
import rider.Rider
import scala.math.Ordering.{OptionOrdering, DoubleOrdering}

class State (val towns: Map[Int, List[Horse]], map: => List[List[Double]], val rider: Rider, val time: Option[Double] = None) {
  
  def moves: List[(Int, Horse)] = {
    if (rider.c == rider.dest){ 
      Nil
    } else {
      val targets = map(rider.c).zipWithIndex.filter{case (value: Double, i: Int) => value >= 0D}.map{case (value: Double, i: Int) => i}
      for{target <- targets
          h <- towns(rider.c) if (h.endurance >= map(rider.c)(target) && !h.visitedTowns.contains(target))
         } yield {
           (target, h)
         }
      }
  }
  
  def move(horse: Horse)(from: Int)(to: Int): State = {
    require(rider.c != rider.dest, s"Rider is already at its destination")
    require(towns(from).contains(horse), s"Town: ${from} does not contain horse: ${horse}")
    require(map(from)(to) >= 0D, s"Town: ${to} is not reachable from town: ${from}")
    require(horse.endurance >= map(from)(to), s"Horse: ${horse} at town: ${from} does not have enough endurance to reach town: ${to}")
    val _horse = Horse(horse.speed, horse.endurance - map(from)(to), horse.visitedTowns + to)
    val etime = time match {
      case Some(x: Double) => x + (map(from)(to)/horse.speed) 
      case None => (map(from)(to)/horse.speed) 
    }
    val currentTownHorses = towns(from).filter(h => h != horse)
    val nextTownHorses = _horse +: towns(to)
    val _towns = towns.updated(from, currentTownHorses).updated(to, nextTownHorses)
    new State(_towns, map, Rider(to, rider.dest), Some(etime))
  }
  
  def terminated: Boolean = if(moves.isEmpty) true else false
  override def toString(): String = {
    s"State(${towns}, ${map}, ${rider}, ${time})"
  }
}

object DoubleOptionOrdering extends OptionOrdering[Double] {
  def optionOrdering: Ordering[Double] = Ordering.Double
}

object TimeOrdering extends Ordering[State]{
  def compare(x: State, y: State): Int = DoubleOptionOrdering.compare(x.time, y.time)
}

object OptionTimeOrdering extends OptionOrdering[State] {
  def optionOrdering: Ordering[State] = TimeOrdering
}