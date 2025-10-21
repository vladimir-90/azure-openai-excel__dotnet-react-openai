import { useState } from 'react';

interface QuestionFormProps {
	aiModels: string[];
	testDataSets: string[];
}

function QuestionForm({ aiModels, testDataSets }: QuestionFormProps) {
	const [selectedModel, setSelectedModel] = useState<string>(
		aiModels.length > 0 ? aiModels[0] : ''
	);
	const [selectedDataSet, setSelectedDataSet] = useState<string>(
		testDataSets.length > 0 ? testDataSets[0] : ''
	);
	const [question, setQuestion] = useState<string>('');

	const handleGoClick = () => {
		// to_do
	};

	return (
		<div className="form-container">
			<div className="form-group">
				<label htmlFor="model-select">Pick a model</label>
				<select
					id="model-select"
					value={selectedModel}
					onChange={(e) => setSelectedModel(e.target.value)}
					className="form-select"
				>
					{aiModels.map((model) => (
						<option key={model} value={model}>
							{model}
						</option>
					))}
				</select>
			</div>

			<div className="form-group">
				<label htmlFor="dataset-select">Pick a file for analysis</label>
				<select
					id="dataset-select"
					value={selectedDataSet}
					onChange={(e) => setSelectedDataSet(e.target.value)}
					className="form-select"
				>
					{testDataSets.map((dataSet) => (
						<option key={dataSet} value={dataSet}>
							{dataSet}
						</option>
					))}
				</select>
			</div>

			<div className="form-group">
				<label htmlFor="question-textarea">Your question</label>
				<textarea
					id="question-textarea"
					value={question}
					onChange={(e) => setQuestion(e.target.value)}
					className="form-textarea"
					placeholder="Enter your question here..."
					rows={4}
				/>
			</div>

			<div className="form-group">
				<button
					onClick={handleGoClick}
					className="go-button"
					disabled={!selectedModel || !selectedDataSet}
				>
					Go
				</button>
			</div>
		</div>
	);
}

export default QuestionForm;
