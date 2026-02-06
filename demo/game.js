// Cat Football Game - Playable Demo
// Demonstrates core gameplay mechanics

const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

// Game Constants
const MATCH_DURATION = 180; // 3 minutes
const BALL_RADIUS = 8;
const CAT_RADIUS = 15;
const BALL_MAX_SPEED = 12;
const CAT_BASE_SPEED = 3;
const CAT_SPRINT_MULTIPLIER = 1.8;
const STAMINA_MAX = 100;
const STAMINA_DRAIN_RATE = 15; // per second
const STAMINA_REGEN_RATE = 10; // per second
const BALL_FRICTION = 0.97;
const MEOWMENTUM_THRESHOLDS = [3, 6, 10, 15, 20];

// Game State
let gameState = {
    matchTime: MATCH_DURATION,
    homeScore: 0,
    awayScore: 0,
    matchInProgress: true,
    meowmentumChain: 0,
    meowmentumLevel: 0,
    lastActionTime: Date.now()
};

// Ball Object
let ball = {
    x: canvas.width / 2,
    y: canvas.height / 2,
    vx: 0,
    vy: 0,
    radius: BALL_RADIUS,
    possessed: false,
    possessor: null
};

// Player Cat
let player = {
    x: 150,
    y: canvas.height / 2,
    vx: 0,
    vy: 0,
    radius: CAT_RADIUS,
    stamina: STAMINA_MAX,
    speed: CAT_BASE_SPEED,
    name: "Whiskers",
    color: "#FF8C42",
    hasBall: false
};

// AI Cat
let ai = {
    x: canvas.width - 150,
    y: canvas.height / 2,
    vx: 0,
    vy: 0,
    radius: CAT_RADIUS,
    stamina: STAMINA_MAX,
    speed: CAT_BASE_SPEED * 0.8,
    name: "Shadow",
    color: "#6C5CE7",
    hasBall: false
};

// Goals
const homeGoal = { x: 0, y: canvas.height / 2, width: 20, height: 150 };
const awayGoal = { x: canvas.width - 20, y: canvas.height / 2, width: 20, height: 150 };

// Input State
let keys = {};

// Event Listeners
window.addEventListener('keydown', (e) => {
    keys[e.key.toLowerCase()] = true;

    if (e.key === 'r' || e.key === 'R') {
        restartMatch();
    }
});

window.addEventListener('keyup', (e) => {
    keys[e.key.toLowerCase()] = false;
});

// Main Game Loop
function gameLoop() {
    update();
    render();
    requestAnimationFrame(gameLoop);
}

// Update Game State
function update() {
    if (!gameState.matchInProgress) return;

    // Update match timer
    gameState.matchTime -= 1/60;
    if (gameState.matchTime <= 0) {
        gameState.matchTime = 0;
        endMatch();
    }

    // Update player input
    updatePlayerInput();

    // Update AI
    updateAI();

    // Update ball physics
    updateBall();

    // Check ball possession
    checkBallPossession();

    // Check goals
    checkGoals();

    // Update Meowmentum
    updateMeowmentum();

    // Update UI
    updateUI();
}

// Update Player Input
function updatePlayerInput() {
    let moveX = 0;
    let moveY = 0;

    // Movement
    if (keys['w']) moveY -= 1;
    if (keys['s']) moveY += 1;
    if (keys['a']) moveX -= 1;
    if (keys['d']) moveX += 1;

    // Normalize diagonal movement
    if (moveX !== 0 && moveY !== 0) {
        const length = Math.sqrt(moveX * moveX + moveY * moveY);
        moveX /= length;
        moveY /= length;
    }

    // Sprint
    let isSprinting = keys['shift'] && player.stamina > 0;
    let currentSpeed = player.speed;

    if (isSprinting) {
        currentSpeed *= CAT_SPRINT_MULTIPLIER;
        player.stamina -= STAMINA_DRAIN_RATE / 60;
        if (player.stamina < 0) player.stamina = 0;
    } else {
        player.stamina += STAMINA_REGEN_RATE / 60;
        if (player.stamina > STAMINA_MAX) player.stamina = STAMINA_MAX;
    }

    // Apply Meowmentum bonus
    const meowmentumBonus = getMeowmentumBonus();
    currentSpeed *= (1 + meowmentumBonus);

    // Update velocity
    player.vx = moveX * currentSpeed;
    player.vy = moveY * currentSpeed;

    // Update position
    player.x += player.vx;
    player.y += player.vy;

    // Keep in bounds
    player.x = Math.max(player.radius, Math.min(canvas.width - player.radius, player.x));
    player.y = Math.max(player.radius, Math.min(canvas.height - player.radius, player.y));

    // Actions
    if (keys[' '] && player.hasBall) {
        shootBall(player, 1.0);
        keys[' '] = false;
        addMeowmentumChain(2, "Shot on goal!");
    }

    if (keys['e'] && player.hasBall) {
        shootBall(player, 1.5);
        keys['e'] = false;
        addMeowmentumChain(2, "Power shot!");
    }

    // Move ball with player if possessed
    if (player.hasBall) {
        ball.x = player.x;
        ball.y = player.y;
    }
}

// Simple AI
function updateAI() {
    // AI chases ball
    const dx = ball.x - ai.x;
    const dy = ball.y - ai.y;
    const distance = Math.sqrt(dx * dx + dy * dy);

    if (distance > 5) {
        ai.vx = (dx / distance) * ai.speed;
        ai.vy = (dy / distance) * ai.speed;
    } else {
        ai.vx = 0;
        ai.vy = 0;
    }

    ai.x += ai.vx;
    ai.y += ai.vy;

    // Keep in bounds
    ai.x = Math.max(ai.radius, Math.min(canvas.width - ai.radius, ai.x));
    ai.y = Math.max(ai.radius, Math.min(canvas.height - ai.radius, ai.y));

    // AI shoots if has ball and close to goal
    if (ai.hasBall && ai.x > canvas.width / 2) {
        shootBall(ai, 1.0);
    }

    // Move ball with AI if possessed
    if (ai.hasBall) {
        ball.x = ai.x;
        ball.y = ai.y;
    }
}

// Update Ball Physics
function updateBall() {
    if (ball.possessed) return;

    // Apply friction
    ball.vx *= BALL_FRICTION;
    ball.vy *= BALL_FRICTION;

    // Stop if very slow
    if (Math.abs(ball.vx) < 0.1) ball.vx = 0;
    if (Math.abs(ball.vy) < 0.1) ball.vy = 0;

    // Update position
    ball.x += ball.vx;
    ball.y += ball.vy;

    // Bounce off walls
    if (ball.x - ball.radius < 0 || ball.x + ball.radius > canvas.width) {
        ball.vx *= -0.8;
        ball.x = Math.max(ball.radius, Math.min(canvas.width - ball.radius, ball.x));
    }
    if (ball.y - ball.radius < 0 || ball.y + ball.radius > canvas.height) {
        ball.vy *= -0.8;
        ball.y = Math.max(ball.radius, Math.min(canvas.height - ball.radius, ball.y));
    }
}

// Check Ball Possession
function checkBallPossession() {
    // Player possession
    const playerDist = Math.sqrt((player.x - ball.x) ** 2 + (player.y - ball.y) ** 2);
    if (playerDist < player.radius + ball.radius + 5 && !ball.possessed) {
        takeBall(player);
    }

    // AI possession
    const aiDist = Math.sqrt((ai.x - ball.x) ** 2 + (ai.y - ball.y) ** 2);
    if (aiDist < ai.radius + ball.radius + 5 && !ball.possessed) {
        takeBall(ai);
    }
}

// Take Ball
function takeBall(cat) {
    ball.possessed = true;
    ball.possessor = cat;
    cat.hasBall = true;
    ball.vx = 0;
    ball.vy = 0;

    if (cat === player) {
        addMeowmentumChain(1, "Ball control!");
    } else {
        // AI takes ball - break player's chain
        resetMeowmentumChain();
    }
}

// Shoot Ball
function shootBall(cat, powerMultiplier) {
    if (!cat.hasBall) return;

    // Calculate direction to goal
    const targetX = cat === player ? canvas.width : 0;
    const targetY = canvas.height / 2 + (Math.random() - 0.5) * 100;

    const dx = targetX - cat.x;
    const dy = targetY - cat.y;
    const distance = Math.sqrt(dx * dx + dy * dy);

    // Shoot
    const power = 8 * powerMultiplier;
    ball.vx = (dx / distance) * power;
    ball.vy = (dy / distance) * power;

    // Release ball
    ball.possessed = false;
    ball.possessor = null;
    cat.hasBall = false;
}

// Check Goals
function checkGoals() {
    // Home goal (left)
    if (ball.x < homeGoal.width &&
        ball.y > homeGoal.y - homeGoal.height / 2 &&
        ball.y < homeGoal.y + homeGoal.height / 2) {

        gameState.awayScore++;
        showMessage("⚽ AWAY GOAL! AI scores!");
        resetBall();
        resetMeowmentumChain();
    }

    // Away goal (right)
    if (ball.x > canvas.width - awayGoal.width &&
        ball.y > awayGoal.y - awayGoal.height / 2 &&
        ball.y < awayGoal.y + awayGoal.height / 2) {

        gameState.homeScore++;
        showMessage("🎉 HOME GOAL! You scored!");
        addMeowmentumChain(5, "GOAL!!!");
        resetBall();
    }
}

// Reset Ball
function resetBall() {
    ball.x = canvas.width / 2;
    ball.y = canvas.height / 2;
    ball.vx = 0;
    ball.vy = 0;
    ball.possessed = false;
    ball.possessor = null;
    player.hasBall = false;
    ai.hasBall = false;

    // Reset positions
    player.x = 150;
    player.y = canvas.height / 2;
    ai.x = canvas.width - 150;
    ai.y = canvas.height / 2;
}

// Meowmentum System
function updateMeowmentum() {
    // Decay chain over time
    const timeSinceAction = Date.now() - gameState.lastActionTime;
    if (timeSinceAction > 5000 && gameState.meowmentumChain > 0) {
        gameState.meowmentumChain -= 0.5 / 60;
        if (gameState.meowmentumChain < 0) gameState.meowmentumChain = 0;
    }

    // Update level
    let newLevel = 0;
    for (let i = 0; i < MEOWMENTUM_THRESHOLDS.length; i++) {
        if (gameState.meowmentumChain >= MEOWMENTUM_THRESHOLDS[i]) {
            newLevel = i + 1;
        }
    }

    if (newLevel !== gameState.meowmentumLevel) {
        gameState.meowmentumLevel = newLevel;
        if (newLevel > 0) {
            showMessage(`✨ Meowmentum Level ${newLevel}! +${getMeowmentumBonus() * 100}% bonus`);
        }
    }
}

function addMeowmentumChain(points, action) {
    gameState.meowmentumChain += points;
    gameState.lastActionTime = Date.now();
}

function resetMeowmentumChain() {
    gameState.meowmentumChain = 0;
    gameState.meowmentumLevel = 0;
    showMessage("💔 Meowmentum chain broken!");
}

function getMeowmentumBonus() {
    const bonuses = [0, 0.05, 0.10, 0.15, 0.20, 0.25];
    return bonuses[gameState.meowmentumLevel] || 0;
}

// End Match
function endMatch() {
    gameState.matchInProgress = false;

    let result = "";
    if (gameState.homeScore > gameState.awayScore) {
        result = "🎉 YOU WIN!";
    } else if (gameState.homeScore < gameState.awayScore) {
        result = "😿 YOU LOSE!";
    } else {
        result = "🤝 DRAW!";
    }

    showMessage(`⏱️ TIME'S UP! ${result} Final Score: ${gameState.homeScore} - ${gameState.awayScore}. Press R to restart.`);
}

// Restart Match
function restartMatch() {
    gameState.matchTime = MATCH_DURATION;
    gameState.homeScore = 0;
    gameState.awayScore = 0;
    gameState.matchInProgress = true;
    gameState.meowmentumChain = 0;
    gameState.meowmentumLevel = 0;
    player.stamina = STAMINA_MAX;
    resetBall();
    showMessage("🔄 Match restarted! Good luck!");
}

// Show Message
function showMessage(message) {
    const msgEl = document.getElementById('statusMessage');
    msgEl.textContent = message;
    msgEl.classList.add('show');
    msgEl.classList.add('pulse');

    setTimeout(() => {
        msgEl.classList.remove('show');
        msgEl.classList.remove('pulse');
    }, 3000);
}

// Update UI
function updateUI() {
    // Score
    document.getElementById('scoreDisplay').textContent =
        `${gameState.homeScore} - ${gameState.awayScore}`;

    // Time
    const minutes = Math.floor(gameState.matchTime / 60);
    const seconds = Math.floor(gameState.matchTime % 60);
    document.getElementById('timeDisplay').textContent =
        `${minutes}:${seconds.toString().padStart(2, '0')}`;

    // Possession
    let possession = "None";
    if (player.hasBall) possession = "You (Whiskers)";
    if (ai.hasBall) possession = "AI (Shadow)";
    document.getElementById('possessionDisplay').textContent = possession;

    // Meowmentum
    const maxChain = MEOWMENTUM_THRESHOLDS[MEOWMENTUM_THRESHOLDS.length - 1];
    const percentage = Math.min(100, (gameState.meowmentumChain / maxChain) * 100);
    document.getElementById('meowmentumFill').style.width = `${percentage}%`;
    document.getElementById('meowmentumText').textContent =
        `Level ${gameState.meowmentumLevel}`;
    document.getElementById('chainDisplay').textContent =
        Math.floor(gameState.meowmentumChain);
    document.getElementById('bonusDisplay').textContent =
        `+${(getMeowmentumBonus() * 100).toFixed(0)}%`;

    // Player stats
    document.getElementById('staminaDisplay').textContent =
        `${Math.floor(player.stamina)}%`;

    const currentSpeed = player.speed * (1 + getMeowmentumBonus());
    document.getElementById('speedDisplay').textContent =
        currentSpeed.toFixed(1);

    let status = "Normal";
    if (player.stamina < 30) status = "😓 Tired";
    if (player.stamina === 0) status = "😴 Exhausted";
    if (gameState.meowmentumLevel >= 3) status = "🔥 On Fire!";
    document.getElementById('statusDisplay').textContent = status;
}

// Render Game
function render() {
    // Clear canvas
    ctx.fillStyle = '#88dd88';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw field markings
    drawField();

    // Draw goals
    drawGoals();

    // Draw ball trail
    drawBallTrail();

    // Draw ball
    drawBall();

    // Draw cats
    drawCat(player);
    drawCat(ai);

    // Draw Meowmentum effect
    if (gameState.meowmentumLevel > 0) {
        drawMeowmentumEffect(player);
    }
}

// Draw Field
function drawField() {
    ctx.strokeStyle = 'white';
    ctx.lineWidth = 3;

    // Center line
    ctx.beginPath();
    ctx.moveTo(canvas.width / 2, 0);
    ctx.lineTo(canvas.width / 2, canvas.height);
    ctx.stroke();

    // Center circle
    ctx.beginPath();
    ctx.arc(canvas.width / 2, canvas.height / 2, 60, 0, Math.PI * 2);
    ctx.stroke();

    // Penalty areas
    ctx.strokeRect(0, canvas.height / 2 - 100, 120, 200);
    ctx.strokeRect(canvas.width - 120, canvas.height / 2 - 100, 120, 200);
}

// Draw Goals
function drawGoals() {
    ctx.fillStyle = 'rgba(255, 255, 255, 0.8)';

    // Home goal
    ctx.fillRect(
        homeGoal.x,
        homeGoal.y - homeGoal.height / 2,
        homeGoal.width,
        homeGoal.height
    );

    // Away goal
    ctx.fillRect(
        awayGoal.x,
        awayGoal.y - awayGoal.height / 2,
        awayGoal.width,
        awayGoal.height
    );
}

// Draw Ball
function drawBall() {
    ctx.fillStyle = 'white';
    ctx.strokeStyle = '#333';
    ctx.lineWidth = 2;

    ctx.beginPath();
    ctx.arc(ball.x, ball.y, ball.radius, 0, Math.PI * 2);
    ctx.fill();
    ctx.stroke();

    // Ball pattern
    ctx.fillStyle = '#333';
    ctx.beginPath();
    ctx.arc(ball.x - 3, ball.y - 3, 2, 0, Math.PI * 2);
    ctx.fill();
    ctx.beginPath();
    ctx.arc(ball.x + 3, ball.y + 3, 2, 0, Math.PI * 2);
    ctx.fill();
}

// Draw Ball Trail
function drawBallTrail() {
    const speed = Math.sqrt(ball.vx * ball.vx + ball.vy * ball.vy);
    if (speed > 4) {
        ctx.strokeStyle = 'rgba(255, 255, 255, 0.3)';
        ctx.lineWidth = ball.radius;
        ctx.beginPath();
        ctx.moveTo(ball.x, ball.y);
        ctx.lineTo(ball.x - ball.vx * 2, ball.y - ball.vy * 2);
        ctx.stroke();
    }
}

// Draw Cat
function drawCat(cat) {
    // Shadow
    ctx.fillStyle = 'rgba(0, 0, 0, 0.2)';
    ctx.beginPath();
    ctx.ellipse(cat.x, cat.y + cat.radius, cat.radius, cat.radius / 3, 0, 0, Math.PI * 2);
    ctx.fill();

    // Body
    ctx.fillStyle = cat.color;
    ctx.strokeStyle = '#333';
    ctx.lineWidth = 2;
    ctx.beginPath();
    ctx.arc(cat.x, cat.y, cat.radius, 0, Math.PI * 2);
    ctx.fill();
    ctx.stroke();

    // Face
    ctx.fillStyle = 'white';
    ctx.beginPath();
    ctx.arc(cat.x, cat.y + 3, cat.radius * 0.6, 0, Math.PI * 2);
    ctx.fill();

    // Eyes
    ctx.fillStyle = '#333';
    ctx.beginPath();
    ctx.arc(cat.x - 5, cat.y, 3, 0, Math.PI * 2);
    ctx.fill();
    ctx.beginPath();
    ctx.arc(cat.x + 5, cat.y, 3, 0, Math.PI * 2);
    ctx.fill();

    // Ears
    ctx.fillStyle = cat.color;
    ctx.beginPath();
    ctx.moveTo(cat.x - 8, cat.y - cat.radius);
    ctx.lineTo(cat.x - 12, cat.y - cat.radius - 8);
    ctx.lineTo(cat.x - 4, cat.y - cat.radius + 2);
    ctx.fill();
    ctx.stroke();
    ctx.beginPath();
    ctx.moveTo(cat.x + 8, cat.y - cat.radius);
    ctx.lineTo(cat.x + 12, cat.y - cat.radius - 8);
    ctx.lineTo(cat.x + 4, cat.y - cat.radius + 2);
    ctx.fill();
    ctx.stroke();

    // Name tag
    ctx.fillStyle = 'rgba(0, 0, 0, 0.7)';
    ctx.fillRect(cat.x - 25, cat.y - cat.radius - 25, 50, 15);
    ctx.fillStyle = 'white';
    ctx.font = '10px Arial';
    ctx.textAlign = 'center';
    ctx.fillText(cat.name, cat.x, cat.y - cat.radius - 15);

    // Stamina bar (player only)
    if (cat === player) {
        const barWidth = 30;
        const barHeight = 4;
        const staminaPercent = cat.stamina / STAMINA_MAX;

        ctx.fillStyle = '#333';
        ctx.fillRect(cat.x - barWidth / 2, cat.y + cat.radius + 5, barWidth, barHeight);

        ctx.fillStyle = staminaPercent > 0.5 ? '#4CAF50' : staminaPercent > 0.2 ? '#FFC107' : '#F44336';
        ctx.fillRect(cat.x - barWidth / 2, cat.y + cat.radius + 5, barWidth * staminaPercent, barHeight);
    }
}

// Draw Meowmentum Effect
function drawMeowmentumEffect(cat) {
    const time = Date.now() * 0.005;
    const levels = [
        { color: 'rgba(102, 126, 234, 0.3)', size: 1.2 },
        { color: 'rgba(102, 126, 234, 0.4)', size: 1.4 },
        { color: 'rgba(118, 75, 162, 0.5)', size: 1.6 },
        { color: 'rgba(255, 215, 0, 0.6)', size: 1.8 },
        { color: 'rgba(255, 105, 180, 0.7)', size: 2.0 }
    ];

    const level = levels[gameState.meowmentumLevel - 1];
    if (level) {
        ctx.strokeStyle = level.color;
        ctx.lineWidth = 3;
        ctx.beginPath();
        ctx.arc(cat.x, cat.y, cat.radius * level.size * (1 + Math.sin(time) * 0.1), 0, Math.PI * 2);
        ctx.stroke();

        // Paw print trails
        for (let i = 0; i < 3; i++) {
            const angle = time + i * Math.PI * 2 / 3;
            const x = cat.x + Math.cos(angle) * cat.radius * 1.5;
            const y = cat.y + Math.sin(angle) * cat.radius * 1.5;

            ctx.fillStyle = level.color;
            ctx.font = '12px Arial';
            ctx.fillText('🐾', x, y);
        }
    }
}

// Start Game
showMessage("⚽ Welcome to Cat Football! Use WASD to move, Space to shoot. Good luck! 🐱");
gameLoop();
