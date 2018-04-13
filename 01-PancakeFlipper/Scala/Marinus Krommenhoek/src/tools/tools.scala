package tools
import scala.io.{Source, BufferedSource}
import java.io.{FileWriter}

class Tools{
}

object Tools{
  def pancakes(format: String): Vector[Boolean] = {
    format.map( f => if(f=='1') true else false).toVector
  }
  def paddle(length: Int, width: Int, pos: Int): Vector[Boolean] = {
    val left = pos
    val middle = width
    val right = length - (left + middle)
    Vector.fill(left)(false) ++ Vector.fill(middle)(true) ++ Vector.fill(right)(false)
  } 
  def flip(pancakes: Vector[Boolean], paddle: Vector[Boolean]): Vector[Boolean] ={
    def xor(x: Boolean, y: Boolean): Boolean = (x || y) && (!(x && y))
    (pancakes zip paddle).map{case (pan: Boolean, pad: Boolean) => xor(pan, pad)}
  }
  def check(pancakes: Vector[Boolean], paddleWidth: Int): Option[Boolean] = {
    val index = pancakes.indexWhere(p => !p)
    if(index == -1){
      Some(true)
    } else if((pancakes.length - index) >= paddleWidth){
      Some(false)
    } else {
      None
    }
  }
  def solve(pancakes: Vector[Boolean], paddleWidth: Int): Option[Int] = {
    def search(pancakes: Vector[Boolean], count: Int): Option[Int] = {
      check(pancakes, paddleWidth) match {
        case Some(true) => Some(count)
        case Some(false) => search(flip(pancakes, paddle(pancakes.length, paddleWidth, pancakes.indexWhere(p => !p))), count + 1)
        case None => None
      }
    }
    search(pancakes, 0)
  }
  def input(filename: String): Iterator[String] = {
    Source.fromFile(filename).getLines().drop(1)
  }
  val input_ = (filename: String) => Source.fromFile(filename).getLines().drop(1)
  def parse(s: String):(String, Int) = {
    val list = s.split(" ").toList
    (list(0), list(1).toInt)
  }
  val parse_ = (s: String) => {val list = s.split(" ").toList; (list(0), list(1))}
  def process(lines: Iterator[String]):Iterator[Option[Int]] = {
    lines.map(f => parse(f)).map{case (s: String, i: Int) => solve(pancakes(s), i)}
  }
  val process_ = (lines: Iterator[String]) => lines.map(f => parse(f)).map{case (s: String, i: Int) => solve(pancakes(s), i)}
  def output(filename: String): Iterator[String] = {
    val out = input_ andThen process_
    out(filename).zipWithIndex.map{case (Some(n: Int), i: Int) => s"Case #$i: $n\n"; case (None, i: Int) => s"Case #$i: IMPOSSIBLE\n"} 
  }
  def write(inputf: String, outputf: String) = {
    val writer = new FileWriter(outputf)
    for(line <- output(inputf)) writer.write(line)
  }
} 