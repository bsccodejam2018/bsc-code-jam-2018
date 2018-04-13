import tools.Tools.{pancakes, output}
import java.lang.Thread
import java.nio.file

object Solution extends App {
  // val classLoader = this.getClass().getClassLoader() // Learned about using getClass() for reflection
  val classLoader = Thread.currentThread().getContextClassLoader // Better, 
  val uri = classLoader.getResource("data/A-small-practice.in")
  for(line <- output(uri.getPath())) print(line)
}