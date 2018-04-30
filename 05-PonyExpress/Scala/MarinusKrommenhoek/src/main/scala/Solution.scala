import inputs.Input
import algorithm.Algorithm
import java.io.FileWriter

object Solution extends App {

  val data = Input.parse("C:/Users/MarinusKrommenhoek/Documents/GitHub/bsc-code-jam-2018/05-PonyExpress/Input/C-small-practice.in")
  def output: String = {
    data.
    map{case (i: Int, m: Map[String, Stream[String]]) => (i, new Algorithm(m))}.
    toSeq.
    sortBy{case (i: Int, alg: Algorithm) => i}.
    map{case (i: Int, alg: Algorithm) => s"Case #${i}: ${alg.output}"}.
    reduce((rs, s) => rs + "\r\n" + s)
  }
  print(output)
  val writer = new FileWriter("C:/Users/MarinusKrommenhoek/Desktop/sol.txt")
  writer.write(output)
  writer.close()
}