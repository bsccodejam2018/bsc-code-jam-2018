package tools
import scala.math

class Tools { 
}

object Tools {
  type Bits = Vector[Boolean]
  
  def hasJamCoinForm(b: Bits): Boolean = (b.head && b.last)
  
  def nonTrivialFactor(base: Int, b: Bits): Option[Int] = {
    def isEven(num: BigInt): Boolean = num % 2 == 0
    def isOdd(num: BigInt): Boolean = num % 2 != 0
    
    def number(base: Int, b: Bits): BigInt = {
      b.indices.filter(i => b(i) == true).map(i => math.pow(base, i)).sum.toInt
     } 
    
    val num = number(base, b)
    val max = math.ceil(math.sqrt(num.toDouble)).toInt
    if(num == 0 || num == 1 || num == 2 || num == 3){ 
      None
    }else if(isEven(num)){
      Some(2)
    } else {
      (3 to max by 2 par).find(n => num % n == 0)
    } 
  }
  
  def emptyBits(l: Int): Bits = Vector.fill(l)(false)
  
  def singleBit(pos: Int, l: Int): Bits = {
    val bits = emptyBits(l)
    bits.indices.map{(f: Int) => if(f == pos) true else bits(f)}.toVector
  }
  
  def add(x: Bits, y: Bits): (Bits, Boolean) = {
    def xor(x: Boolean, y: Boolean): Boolean = (x || y) && (!( x && y))
    def carry(x: Boolean, y: Boolean): Boolean = x && y
    
    def sum(x: Bits, y: Bits, r: Bits, c: Boolean): (Bits, Boolean) = {
      if(x.isEmpty) (r, c) else {
        val xhead = x.head
        val yhead = y.head
        sum(x.tail, y.tail, r :+ xor(xor(xhead, c), yhead), carry(xhead, c) || carry(xor(xhead, c), yhead))
        }
    }
    sum(x, y, Vector.empty, false)
  }
  
  def search(l: Int, n: Int): Vector[(Vector[Boolean], List[Int])] = {
    val start = add(singleBit(0, l), singleBit(l-1, l))._1
    val offset = singleBit(1, l)
    def solution(bits: Bits, answer: Vector[(Vector[Boolean], List[Int])]):Vector[(Vector[Boolean], List[Int])] = {
      if(answer.length >= n) answer else {
         def searchResults: List[Int] = {
           def rep(base: Int, answer: List[Int]): List[Int] = {
             nonTrivialFactor(base, bits) match {
               case Some(i: Int) => if(base < 10) rep(base + 1, i +: answer) else i +: answer
               case None => answer
             }
           }
           rep(2, Nil)
         }
         val (sum, overflow) = add(bits, offset)
         val results = searchResults
         if(results.length < (2 to 10).length){
           if(overflow) answer else solution(sum, answer)
         } else {
           if(overflow) (bits, results) +: answer else solution(sum, (bits, results) +: answer)
         }
        }
      }
    solution(start, Vector.empty)
  }
  
  def formatForOutput(answer: Vector[(Bits, List[Int])]): String = {
    def bitsToString(b: Bits): String = {
       b.map(f => if(f) 1 else 0).reverse.mkString("")
    }
    
    def factorsToString(factors: List[Int]): String = {
      factors.mkString(" ")
    }
    
    def optionToString(option: (Bits, List[Int])): String = {
      val bits = bitsToString(option._1)
      val list = factorsToString(option._2)
      s"$bits $list"
    }
    answer.map(f => optionToString(f)).mkString("\n")
  }
}