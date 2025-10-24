import {
	REASONING_EFFORT_LEVELS,
	type ReasoningEffortLevel,
} from '../services/excel-analysis.service';

interface ReasoningEffortLevelPickerProps {
	value: ReasoningEffortLevel | '';
	onChange: (level: ReasoningEffortLevel) => void;
}

function ReasoningEffortLevelPicker({
	value,
	onChange,
}: ReasoningEffortLevelPickerProps) {
	return (
		<div className="d-flex align-items-center gap-3">
			<label className="form-label mb-0 text-nowrap">
				🧠 Reasoning Effort Level
			</label>
			<div className="d-flex align-items-center gap-3">
				{REASONING_EFFORT_LEVELS.map(({ level, tokens }) => (
					<div key={level} className="form-check">
						<input
							type="radio"
							id={`reasoning-${level}`}
							name="reasoning-level"
							value={level}
							checked={value === level}
							onChange={(e) =>
								onChange(e.target.value as ReasoningEffortLevel)
							}
							className="form-check-input"
							required
						/>
						<label
							htmlFor={`reasoning-${level}`}
							className="form-check-label me-1"
						>
							{level}
						</label>
						<small className="text-muted">({tokens} tokens)</small>
					</div>
				))}
			</div>
		</div>
	);
}
export default ReasoningEffortLevelPicker;
