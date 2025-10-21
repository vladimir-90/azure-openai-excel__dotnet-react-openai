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
		<div className="card p-4" style={{ maxWidth: 600, minWidth: 400 }}>
			<div className="mb-3">
				<label htmlFor="model-select" className="form-label">
					Pick a model
				</label>
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

			<div className="mb-3">
				<label htmlFor="dataset-select" className="form-label">
					Pick a file for analysis
				</label>
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

			<div className="mb-3">
				<label htmlFor="question-textarea" className="form-label">
					Ask a question
				</label>
				<textarea
					id="question-textarea"
					value={question}
					onChange={(e) => setQuestion(e.target.value)}
					className="form-control"
					rows={5}
				/>
			</div>

			<div className="d-flex justify-content-end">
				<button
					onClick={handleGoClick}
					className="btn btn-primary"
					disabled={!selectedModel || !selectedDataSet}
				>
					Get answer
				</button>
			</div>
		</div>
	);
}

export default QuestionForm;
