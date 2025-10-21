import { useState } from 'react';

interface QuestionFormProps {
	aiModels: string[];
	testDataSets: string[];
}

function QuestionForm({ aiModels, testDataSets }: QuestionFormProps) {
	const [selectedModel, setSelectedModel] = useState<string>('');
	const [selectedDataSet, setSelectedDataSet] = useState<string>('');
	const [question, setQuestion] = useState<string>('');

	const handleGoClick = () => {
		// to_do
	};

	return (
		<div className="card border-0 shadow-lg question-form-card">
			{/* Header */}
			<div className="card-header question-form-header text-white text-center py-3 border-0">
				<h4 className="mb-0 fw-light">Excel Analysis AI</h4>
				<small className="opacity-75">
					Ask questions about your data
				</small>
			</div>

			{/* Form Body */}
			<div className="card-body p-4">
				<div className="row g-3">
					{/* Model Selection */}
					<div className="col-md-6">
						<div className="form-floating">
							<select
								id="model-select"
								value={selectedModel}
								onChange={(e) =>
									setSelectedModel(e.target.value)
								}
								className="form-select question-form-select"
							>
								<option value="" disabled>
									Choose model...
								</option>
								{aiModels.map((model) => (
									<option key={model} value={model}>
										{model}
									</option>
								))}
							</select>
							<label
								htmlFor="model-select"
								className="text-muted"
							>
								🤖 AI Model
							</label>
						</div>
					</div>

					{/* Dataset Selection */}
					<div className="col-md-6">
						<div className="form-floating">
							<select
								id="dataset-select"
								value={selectedDataSet}
								onChange={(e) =>
									setSelectedDataSet(e.target.value)
								}
								className="form-select question-form-select"
							>
								<option value="" disabled>
									Choose dataset...
								</option>
								{testDataSets.map((dataSet) => (
									<option key={dataSet} value={dataSet}>
										{dataSet}
									</option>
								))}
							</select>
							<label
								htmlFor="dataset-select"
								className="text-muted"
							>
								📊 Dataset
							</label>
						</div>
					</div>

					{/* Question Input */}
					<div className="col-12 mt-4">
						<div className="form-floating">
							<textarea
								id="question-textarea"
								value={question}
								onChange={(e) => setQuestion(e.target.value)}
								className="form-control question-form-textarea"
								placeholder="What would you like to know about your data?"
							/>
							<label
								htmlFor="question-textarea"
								className="text-muted"
							>
								💭 Your Question
							</label>
						</div>
						<div className="form-text text-muted mt-2">
							Try asking: "What are the top 5 sales by region?" or
							"Show me the average salary by department"
						</div>
					</div>
				</div>
			</div>

			{/* Footer with Action Button */}
			<div className="card-footer bg-light border-0 text-end p-4">
				<button
					onClick={handleGoClick}
					className={`btn btn-lg px-5 fw-semibold question-form-button ${
						selectedModel && selectedDataSet && question.trim()
							? 'question-form-button-enabled'
							: 'question-form-button-disabled'
					}`}
					disabled={
						!selectedModel || !selectedDataSet || !question.trim()
					}
				>
					{!selectedModel || !selectedDataSet || !question.trim() ? (
						<span>Complete the form above</span>
					) : (
						<span className="text-white">🚀 Analyze Data</span>
					)}
				</button>
			</div>
		</div>
	);
}

export default QuestionForm;
