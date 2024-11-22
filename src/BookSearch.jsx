import React,{useState} from "react";
import axios from "axios";
import './booksearch.css'
const BookSearch=()=>{

    const [query,setQuery] = useState("")

    const[books,setBooks] = useState([])
    const [loading,setLoading]  = useState(false)
    const handleSearch= async(event)=>{
        event.preventDefault();
        if(!query)return
        setLoading(true);
        try {
            const response = await axios.get(`https://localhost:7192/api/booksearch/search?query=${query}`)

            setBooks(response.data);
            console.log(response.data)
            console.log(books)
        } catch (error) {
            console.error("Error fetching books: ",error)
        }finally{
            setLoading(false)
        }
      
    }
return(
    <div>
            <h1>Book Search</h1>
            <form onSubmit={handleSearch}>
                <input type="text" value={query} onChange={(e)=>setQuery(e.target.value)} placeholder="Enter book or authors name" className="query"></input>
                <button className="button"> Search </button>
            </form>
            {loading && <p>loading...</p>}
            <div className="box">
                
                    {books.length>0 ? 
                    books.map((book,index)=>(
                        <div className="book" key={book.id || index}>
                         <img src={book.volumeInfo.imageLinks.thumbnail} alt="book/image"></img>
                        <h3>{book.volumeInfo.title}</h3>
                        <p>{book.volumeInfo.firstAuthor}</p>
                        <p>{book.volumeInfo.publishedDate}</p>
                        
                    </div>
                    ))
                    
                    :<p>No books found</p>}
                 
            </div>
    </div>
)

}
export default BookSearch;
