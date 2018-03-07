import scala.io.Source

object Solution extends App {
  
  val filename = "./data/A-small-practice.in"
  val bufferedSource = Source.fromFile(filename)
  
  val list = bufferedSource.getLines.toList
  
  def classify(no: Int): String = {
    
    def scenario(c: Int, l: List[String]): String = {
      if (l.isEmpty){
      s"case #$c: Volunteer cheated!"
      }
      else if(l.length > 1){
        s"case #$c: Bad magician!"
      }
      else{
        s"case #$c: ${l.head}"
      }
    }
    
    val answer1_i = (no - 1)*10
    val answer1 = list.tail(answer1_i).toInt - 1
    
    val answer2_i = no*10 - 5
    val answer2 = list.tail(answer2_i).toInt - 1
    
    val slice1 = list.tail.slice(answer1_i + 1, answer1_i + 5).map(l => l.split(" ")).toList

    
    val slice2 = list.tail.slice(answer2_i + 1, answer2_i + 5).map(l => l.split(" ")).toList

    
    scenario(no, slice1(answer1).intersect(slice2(answer2)).toList)
  }

  
  val outputs = (1 until list.head.toInt + 1).map( i => classify(i))
  
  println(outputs)
  
  bufferedSource.close()
}