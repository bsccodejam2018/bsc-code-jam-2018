import algorithm.Algorithm
import inputs.Input
import state.State

object Solution extends App {
  def output(i: Int, ls: List[State]): String = {
    s"Case #${i}: ${ls.map(s => s.time match {
      case Some(t: Double) => t.toString()
      case _ => ""
    }).mkString(" ")}"
  }
  val _inputs = Input.completeParse("C:/Users/MarinusKrommenhoek/Documents/GitHub/bsc-code-jam-2018/05-PonyExpress/Input/C-small-practice.in")
  val algo = new Algorithm()
  for(c <- _inputs) println(output(c._1, c._2.map(s => algo._solve(s))))
   //val x = _inputs.map( c => (c._1, c._2)).toMap
   //print(algo.solve(_inputs(4)(0)))
}