import React from 'react';
import { useState } from 'react';


export interface QuestionResponse {
    question: string | null;
    response: string | null;
};

function Home() {

    const [chatResponse, setChatResponse] = useState<string|null>(null);

    function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
        e.preventDefault();

        const formData = new FormData(e.currentTarget);
        const formObj = Object.fromEntries(formData.entries());

        fetch("/api/chat", {
            method: 'POST',
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(formObj)
        })
        .then(httpResponse => httpResponse.json())
        .then(obj => setChatResponse(obj.response) )
    }

    return (
      <div>
        <h1>Adventure Works GPT</h1>
        <p>I've got access to Microsoft's "AdventureWorks" Sample product database. <br/>
          Ask me questions and I'll generate SQL to fetch data for you.</p>
        
        <form method="post" onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="question" className="form-label">Question text</label>
            <input type="text" className="form-control" id="question" name="question" aria-describedby="questionHelp" />
            <div id="questionHelp" className="form-text">Enter your question in natural English</div>
        </div>
          <button type="submit" className="btn btn-primary">Submit</button>
        </form>
        { chatResponse && 
        <div className="card mt-3">
          <div className="card-body">
            <h5 className="card-title">Generated SQL</h5>
            <p className="card-text">{chatResponse}</p>
            
        </div>
      </div>
        }
      </div>
    );
}

export default Home;
  