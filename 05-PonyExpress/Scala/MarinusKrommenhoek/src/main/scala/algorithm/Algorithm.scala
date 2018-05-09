package algorithm
import state.State
import state.OptionTimeOrdering
import scala.math
import horse.Horse

class Algorithm {
  def solve(state: State): State = {
    def choices(ss: List[State], answer: List[State] = Nil): List[State] = {
      val _answer = ss.filter(s => s.rider.c == s.rider.dest)
      val _notanswer = ss.filterNot(s => s.terminated)
      if (_notanswer.isEmpty) _answer ++ answer else {
        choices( 
            //_notanswer.map(s => s.moves.map{case (t: Int, h: Horse) => s.move(h)(s.rider.c)(t)}).flatten,
            ss.filterNot(s => s.rider.c == s.rider.dest).map(s => s.moves.map{case (t: Int, h: Horse) => s.move(h)(s.rider.c)(t)}).flatten,
            _answer ++ answer 
            )
      }
    }
    choices(List(state)).filter(s => s.time.isDefined).minBy(s => s.time)
  }
  
  def _solve(s: State): State = {
    def cases(s: State): List[Option[State]] = {
      for{(t, h) <- s.moves} yield {eval(s.move(h)(s.rider.c)(t))}
    }
    def eval(s: State): Option[State] = {
      if (s.rider.c == s.rider.dest) Some(s)
      else if (s.terminated) None
      else {
        def min(ls: List[Option[State]]): Option[State] = if(ls.isEmpty) None else ls.min(OptionTimeOrdering)
        min(cases(s).filter(_.isDefined))
      } 
    }
    
    eval(s).getOrElse(s)
  }

}